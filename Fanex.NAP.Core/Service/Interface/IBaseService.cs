namespace Fanex.NAP.Core.Service.Interface
{
    public interface IBaseService
    {
        IServiceContext ServiceContext { get; set; }

        T Get<T>(string endPoint);
    }
}
