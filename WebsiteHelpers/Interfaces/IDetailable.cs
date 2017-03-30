namespace WebsiteHelpers.Interfaces
{
    public interface IDetailable<TDetail>
    {
        TDetail ToDetail();
    }
}
