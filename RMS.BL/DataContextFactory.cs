using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Threading;
using System.Web;
namespace RMS.BL
{
    public class DataContextFactory
    {
        /// <summary>
        /// Creates a new Data Context for a specific DataContext type
        /// 
        /// Provided merely for compleness sake here - same as new YourDataContext()
        /// </summary>
        /// <typeparam name="RMSDataContext"></typeparam>
        /// <returns></returns>
        public static RMSDataContext GetDataContext<RMSDataContext>()
                where RMSDataContext : DataContext, new()
        {
            return (RMSDataContext)Activator.CreateInstance<RMSDataContext>();
        }

        /// <summary>
        /// Creates a new Data Context for a specific DataContext type with a connection string
        /// 
        /// Provided merely for compleness sake here - same as new YourDataContext()
        /// </summary>
        /// <typeparam name="RMSDataContext"></typeparam>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static RMSDataContext GetDataContext<RMSDataContext>(string connectionString)
                where RMSDataContext : DataContext, new()
        {
            Type t = typeof(RMSDataContext);
            return (RMSDataContext)Activator.CreateInstance(t, connectionString);
        }


        /// <summary>
        /// Creates a ASP.NET Context scoped instance of a DataContext. This static
        /// method creates a single instance and reuses it whenever this method is
        /// called.
        /// 
        /// This version creates an internal request specific key shared key that is
        /// shared by each caller of this method from the current Web request.
        /// </summary>
        

        /// <summary>
        /// Creates a ASP.NET Context scoped instance of a DataContext. This static
        /// method creates a single instance and reuses it whenever this method is
        /// called.
        /// 
        /// This version lets you specify a specific key so you can create multiple 'shared'
        /// DataContexts explicitly.
        /// </summary>
        /// <typeparam name="RMSDataContext"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
       

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="RMSDataContext"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
       

        /// <summary>
        /// Internal method that handles creating a context that is scoped to the HttpContext Items collection
        /// by creating and holding the DataContext there.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        

        /// <summary>
        /// Creates a Thread Scoped DataContext object that can be reused.
        /// The DataContext is stored in Thread local storage.
        /// </summary>
        /// <typeparam name="RMSDataContext"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static RMSDataContext GetThreadScopedDataContext<RMSDataContext>()
                                   where RMSDataContext : DataContext, new()
        {
            return (RMSDataContext)GetThreadScopedDataContextInternal(typeof(RMSDataContext), null, null);
        }


        /// <summary>
        /// Creates a Thread Scoped DataContext object that can be reused.
        /// The DataContext is stored in Thread local storage.
        /// </summary>
        /// <typeparam name="RMSDataContext"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static RMSDataContext GetThreadScopedDataContext<RMSDataContext>(string key)
                                   where RMSDataContext : DataContext, new()
        {
            return (RMSDataContext)GetThreadScopedDataContextInternal(typeof(RMSDataContext), key, null);
        }


        /// <summary>
        /// Creates a Thread Scoped DataContext object that can be reused.
        /// The DataContext is stored in Thread local storage.
        /// </summary>
        /// <typeparam name="RMSDataContext"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        static object GetThreadScopedDataContextInternal(Type type, string key, string ConnectionString)
        {
            if (key == null)
                key = "__WRSCDC_" + Thread.CurrentContext.ContextID.ToString();

            LocalDataStoreSlot threadData = Thread.GetNamedDataSlot(key);
            object context = null;
            if (threadData != null)
                context = Thread.GetData(threadData);

            if (context == null)
            {
                if (ConnectionString == null)
                    context = Activator.CreateInstance(type);
                else
                    context = Activator.CreateInstance(type, ConnectionString);

                if (context != null)
                {
                    if (threadData == null)
                        threadData = Thread.AllocateNamedDataSlot(key);

                    Thread.SetData(threadData, context);
                }
            }

            return context;
        }


        /// <summary>
        /// Returns either Web Request scoped DataContext or a Thread scoped
        /// request object if not running a Web request (ie. HttpContext.Current)
        /// is not available.
        /// </summary>
        /// <typeparam name="RMSDataContext"></typeparam>
        /// <param name="key"></param>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
       

        /// <summary>
        /// Returns either Web Request scoped DataContext or a Thread scoped
        /// request object if not running a Web request (ie. HttpContext.Current)
        /// is not available.
        /// </summary>
        /// <typeparam name="RMSDataContext"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
       
        /// <summary>
        /// Returns either Web Request scoped DataContext or a Thread scoped
        /// request object if not running a Web request (ie. HttpContext.Current)
        /// is not available.
        /// </summary>
        /// <typeparam name="RMSDataContext"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
       

    }
}
