using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shared.Domain;

namespace Shared.Persistency
{
    //public class BaseSavingRepository
    //{
    //    private readonly BaseContext context;

    //    public BaseSavingRepository(BaseContext context)
    //    {
    //        this.context = context;
    //    }
    //    public async Task Save(BaseEntity entity)
    //    {
    //        context.BeginTransaction();

    //        if (entity.State == SaveState.UnSaved)
    //        {
    //            context.Entry(entity).State = EntityState.Added;
    //        }
    //        else
    //        {
    //            context.Entry(entity).State = EntityState.Modified;
    //        }

    //        await context.SaveChangesAsync();
    //        entity.SetSaved();
    //    }

    //}
}