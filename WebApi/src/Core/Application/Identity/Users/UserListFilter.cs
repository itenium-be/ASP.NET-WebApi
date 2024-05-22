namespace FSH.WebApi.Application.Identity.Users;

public class UserListFilter : PaginationFilter
{
    public bool? IsActive { get; set; }

    public override string ToString() => $"Active={IsActive}, Pagination={base.ToString()}";
}