namespace CleanArchitecture.Application.Common.Abstracts.Business
{
    public abstract class BaseService : IBaseService
    {
        #region Dependencies
        protected IServiceProvider ServiceProvider { get; }

        #endregion

        #region Constructor
        protected BaseService(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        #endregion

        #region Methods

        #endregion
    }
}
