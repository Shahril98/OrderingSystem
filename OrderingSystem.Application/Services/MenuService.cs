using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OrderingSystem.Application.Repositories;
using OrderingSystem.Application.Utils;
using OrderingSystem.Infrastructure;
using OrderingSystem.Infrastructure.Databases.OrderingSystem;
using OrderingSystem.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrderingSystem.Application.Services
{
    public interface IMenuService
    {
        Task<MenuDto> AddMenu(AddMenuDto menuDto, Guid userId);
        Task<MenuGroupDto> AddMenuGroup(AddMenuGroupDto addMenuGroup, Guid userId);
        Task AssignImagesToMenu(Guid menuId, List<AddFileResultDto> newImages, List<Guid> existingImageIds);
        Task DeleteMenu(Guid menuId, Guid userId);
        Task DeleteMenuGroup(Guid groupId, Guid userId);
        Task<List<MenuGroupDto>> GetAllMenuGroup();
        Task<List<MenuDto>> GetAllMenus(bool? activeOnly);
        Task<MenuDto> GetMenuById(Guid menuId);
        Task<MenuGroupDto> GetMenuGroupById(Guid groupId);
        Task<List<MenuDto>> GetMenusByGroup(Guid groupId, bool? activeOnly);
        Task<MenuByTypeDto> GetMenusGroupedByTypes(bool? activeOnly);
        Task<MenuDto> UpdateMenu(UpdateMenuDto menuDto, Guid menuId, Guid userId);
        Task<MenuGroupDto> UpdateMenuGroup(UpdateMenuGroupDto menuGroupDto, Guid groupId, Guid userId);
        Task<(List<AddFileResultDto>, HttpMultiStatus)> UploadMenuImages(IFormFileCollection images);
    }

    public class MenuService(IBaseRepository baseRepository, IMenuRepository menuRepository, IFileService fileService, ILogger<MenuService> logger) : IMenuService
    {
        public async Task<MenuDto> AddMenu(AddMenuDto menuDto, Guid userId)
        {
            var toSave = menuDto.Adapt<TblMenu>();
            baseRepository.AddData(toSave);
            await baseRepository.SaveChanges(userId);
            return toSave.Adapt<MenuDto>();
        }

        public async Task<List<MenuDto>> GetAllMenus(bool? activeOnly)
        {
            var result = await menuRepository.GetAllMenu(activeOnly);
            return result;
        }
        public async Task<MenuDto> GetMenuById(Guid menuId)
        {
            var result = await menuRepository.GetMenuById(menuId);
            return result;
        }
        public async Task<MenuDto> UpdateMenu(UpdateMenuDto menuDto, Guid menuId, Guid userId)
        {
            var record = await baseRepository.GetDataById<TblMenu>(menuId);
            var toSave = menuDto.Adapt(record);
            await baseRepository.SaveChanges(userId);
            return toSave.Adapt<MenuDto>();
        }
        public async Task DeleteMenu(Guid menuId, Guid userId)
        {
            using var t = await baseRepository.StartTransaction();
            var menuData = await menuRepository.GetMenuById(menuId);
            var imageIds = menuData.Images.Select(x => x.Id.GetValueOrDefault());
            await menuRepository.DeleteMenuImages(menuId, imageIds);
            await baseRepository.DeleteDataById<TblMenu>(menuId);
            await baseRepository.SaveChanges(userId);
            await baseRepository.CommitTransaction();
        }

        public async Task<MenuByTypeDto> GetMenusGroupedByTypes(bool? activeOnly)
        {
            var allMenu = await menuRepository.GetAllMenu(activeOnly);
            var menuGrouped = allMenu.Adapt<List<MenuDto>>().GroupBy(o => o.MenuType);
            var result = new MenuByTypeDto();
            foreach(var menu in menuGrouped)
            {
                switch (menu.Key)
                {
                    case MenuType.Others:
                        result.Others = [.. menu];
                        break;
                    case MenuType.MainCourse:
                        result.MainCourse = [.. menu];
                        break;
                    case MenuType.Dessert:
                        result.Dessert = [.. menu];
                        break;
                    case MenuType.Drinks:
                        result.Drinks = [.. menu];
                        break;
                }
            }
            return result;
        }
        public async Task<MenuGroupDto> AddMenuGroup(AddMenuGroupDto addMenuGroup, Guid userId)
        {
            var toSave = addMenuGroup.Adapt<TblMenuGroup>();
            baseRepository.AddData(toSave);
            await baseRepository.SaveChanges(userId);
            return toSave.Adapt<MenuGroupDto>();
        }
        public async Task<List<MenuGroupDto>> GetAllMenuGroup()
        {
            var result = await baseRepository.GetAllData<TblMenuGroup>();
            return result.Adapt<List<MenuGroupDto>>(); 
        }
        public async Task<MenuGroupDto> GetMenuGroupById(Guid groupId)
        {
            var result = await baseRepository.GetDataById<TblMenuGroup>(groupId);
            return result.Adapt<MenuGroupDto>();
        }
        public async Task<MenuGroupDto> UpdateMenuGroup(UpdateMenuGroupDto menuGroupDto, Guid groupId, Guid userId)
        {
            var dbData = await baseRepository.GetDataById<TblMenuGroup>(groupId);
            var toUpdate = menuGroupDto.Adapt(dbData);
            baseRepository.UpdateData(toUpdate);
            await baseRepository.SaveChanges(userId);
            return dbData.Adapt<MenuGroupDto>();
        }
        public async Task DeleteMenuGroup(Guid groupId, Guid userId)
        {
            await baseRepository.DeleteDataById<TblMenuGroup>(groupId);
            await baseRepository.SaveChanges(userId);
        }
        public async Task<List<MenuDto>> GetMenusByGroup(Guid groupId, bool? activeOnly)
        {
            Expression<Func<TblMenu, bool>> whereCond;
            if(!activeOnly.HasValue || !activeOnly.Value)
            {
                whereCond = o => o.MenuGroupId == groupId;
            }
            else
            {
                whereCond = o => o.MenuGroupId == groupId && o.MenuStatus != MenuStatus.NotActive;
            }
            var result = await baseRepository.GetAllDataWithCondition<TblMenu>(whereCond);
            return result.Adapt(new List<MenuDto>());
        }

        public async Task<(List<AddFileResultDto>, HttpMultiStatus)> UploadMenuImages(IFormFileCollection images)
        {
            List<string> validExtensions = [".jpg", ".jpeg", ".png"];
            List<AddFileResultDto> result = [];
            if (images is not null  && images.Any())
            {
                if (images.Count > 5)
                    throw new BadRequestException("Had maksimum untuk jumlah imej bagi satu menu adalah 5 imej");
                if (images.Any(o => o.Length > FileSize.MaxMenuImageSize))
                    throw new BadRequestException("Saiz satu fail mesti tidak melebihi 1mb");
                if (images.Any(o => !validExtensions.Contains(Path.GetExtension(o.FileName.ToLower()))))
                    throw new NotValidMediaType($"Fail mesti di dalam format [{string.Join(',', validExtensions)}]");
                if (images.Select(o => o.FileName).Distinct().Count() != images.Count)
                    throw new BadRequestException("Setiap nama imej hendaklah berbeza");
                foreach (var file in images)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var extension = Path.GetExtension(file.FileName);
                    try
                    {
                        var toSave = new AddFileDto(fileName, extension, file.ContentType, file.ToByteArray(), true);
                        result.Add(await fileService.AddFile(toSave));
                    }
                    catch (Exception e)
                    {
                        result.Add(new AddFileResultDto(null, fileName, extension, false));
                        logger.LogError(AppLogEvent.RepositoryError, e, "Fail tidak dapat disimpan di dalam db");
                    }
                }
                if (result.All(o => !o.IsSuccess))
                    throw new NoDataUpdatedException("Tiada imej disimpan");
                else if (result.Any(o => !o.IsSuccess))
                    return (result, HttpMultiStatus.Multi);
                else
                    return (result, HttpMultiStatus.Success);
            }
            else
            {
                return (result,  HttpMultiStatus.Success);
            }
        }
        public async Task AssignImagesToMenu(Guid menuId, List<AddFileResultDto> newImages, List<Guid> existingImageIds)
        {
            var newImageIds = newImages.Where(o => o.IsSuccess).Select(o => o.Id.GetValueOrDefault()).ToList();
            existingImageIds.AddRange(newImageIds);
            var imageFromDb = await baseRepository.GetAllDataWithCondition<TblMenuImage>(o => o.MenuId == menuId);
            var toDelete = imageFromDb.Where(o => !existingImageIds.Contains(o.FileId));
            var toUpdate = imageFromDb.Where(o => existingImageIds.Contains(o.FileId));
            foreach(var image in toUpdate)
            {
                image.Order = existingImageIds.IndexOf(image.FileId) + 1;
            }
            var toAdd = new List<TblMenuImage>();
            var order = 1;
            foreach (var imageId in existingImageIds)
            {
                if(!toUpdate.Any(o => o.FileId == imageId))
                {
                    toAdd.Add(new()
                    {
                        MenuId = menuId,
                        FileId = imageId,
                        Order = order,
                    });
                }
                order++;
            }
            try
            {
                using var t = await baseRepository.StartTransaction();
                baseRepository.DeleteDataBatch(toDelete);
                //baseRepository.UpdateDataBatch(toUpdate);
                baseRepository.AddDataBatch(toAdd);
               await fileService.RemoveFiles(toDelete.Select(o => o.FileId));
                await baseRepository.SaveChanges();
                await baseRepository.CommitTransaction();
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Pemadanan imej ke menu gagal");
                throw new OperationAbortedException();
            }
        }
    }
}
