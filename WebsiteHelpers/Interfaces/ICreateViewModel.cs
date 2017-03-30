namespace WebsiteHelpers.Interfaces
{
    public interface ICreateViewModel<TModel>
    {
        TModel ToItem();
    }
}
