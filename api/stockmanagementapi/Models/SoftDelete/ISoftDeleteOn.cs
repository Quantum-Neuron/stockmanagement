namespace stockmanagementapi.Models.SoftDelete
{
  public interface ISoftDeleteOn
  {
    public bool IsDeleted { get; set; }
  }
}