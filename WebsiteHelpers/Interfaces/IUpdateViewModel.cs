namespace WebsiteHelpers.Interfaces
{
    public interface IUpdateViewModel<TModel>
    {
        int Id { get; set; }

        void UpdateItem(TModel item);
    }
}
