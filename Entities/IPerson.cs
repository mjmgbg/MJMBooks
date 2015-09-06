namespace Entities
{
    public interface IPerson : IBaseObject
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string DisplayFullName { get; }
    }
}