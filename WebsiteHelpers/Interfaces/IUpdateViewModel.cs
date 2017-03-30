namespace WebsiteHelpers.Interfaces
{
    public interface IUpdateViewModel<TModel>
    {
        void UpdateItem(TModel item);
        int Id { get; set; }
    }
}
