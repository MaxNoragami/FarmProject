namespace FarmProject.Domain.Models;

public class Customer(
        string firstName,
        string lastName,
        string email,
        string phoneNum) 
    : Entity
{
    public string FirstName { get; private set; } = firstName;
    public string LastName { get; private set; } = lastName;
    public string Email { get; private set; } = email;
    public string PhoneNum { get; private set; } = phoneNum;
    public List<Order> Orders { get; private set; } = new();
}
