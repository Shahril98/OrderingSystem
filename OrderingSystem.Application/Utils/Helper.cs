using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using OrderingSystem.Infrastructure.Databases.OrderingSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OrderingSystem.Infrastructure;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OrderingSystem.Application.Utils
{   
    public static class Helper
    {
        public static bool StringIsEmail(this string email)
        {
            var e = new EmailAddressAttribute();
            return e.IsValid(email);
        }
        public static bool IsCustomException(this Exception e)
        {
            return e.GetType().IsSubclassOf(typeof(CustomException));
        }
        public static bool IsSystemException(this Exception e)
        {
            return e.GetType().IsSubclassOf(typeof(SystemException));
        }
        public static void PrepareUpdate<TEntity>(this EntityEntry<TEntity> data, params string[] propToUpdate) where TEntity : TblBase
        {
            data.Property(o => o.Id).IsModified = false;
            data.Property(o => o.TimestampUpdated).IsModified = true;
            foreach (var prop in data.Properties.Where(o => propToUpdate.Contains(o.Metadata.Name)))
            {
                prop.IsModified = true;
            }
        }
        public static async Task<int> ExecuteUpdateAsyncCustom<TSource>(this IQueryable<TSource> source,
        Expression<Func<SetPropertyCalls<TSource>, SetPropertyCalls<TSource>>> setPropertyCalls, Guid? userId, bool isDeleted,
        CancellationToken cancellationToken = default) where TSource : TblBaseSoftDelete
        {
            var idT = userId.GetValueOrDefault();
            Expression<Func<SetPropertyCalls<TSource>, SetPropertyCalls<TSource>>> updateExp;
            if (isDeleted)
                updateExp = setter => setter.SetProperty(x => x.TimestampUpdated, DateTimeOffset.UtcNow)
                    .SetProperty(x => x.IsDeleted, true)
                     .SetProperty(x => x.TimestampDeleted, DateTimeOffset.UtcNow);
            else
                updateExp = setter => setter.SetProperty(x => x.TimestampUpdated, DateTimeOffset.UtcNow);
            setPropertyCalls = setPropertyCalls.Update(updateExp.Body, updateExp.Parameters);
            return await source.ExecuteUpdateAsync(setPropertyCalls, cancellationToken);
        }
        public static byte[] ToByteArray(this IFormFile file)
        {
            byte[] fileByteArray;
            using (var item = new MemoryStream())
            {
                file.CopyTo(item);
                fileByteArray = item.ToArray();
            }
            return fileByteArray;
        }
    }
}
