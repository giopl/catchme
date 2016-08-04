using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace CatchMe.Models
{
    public partial class CatchMeDBEntities
    {
        log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override int SaveChanges()
        {
            log.Debug("Saving aLL MY lines for you...");
            try
            {


            return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    log.DebugFormat("Entity of type {0} in state {1} has the following validation errors:",eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        log.DebugFormat("- Property: {0}, Value: {1}, Error: {2}",ve.PropertyName,eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}