namespace SocialMedia.Core.Interfaces.Post;
public interface IPostService
{
    public Task<bool> DeleteAsync(int id);
    public Task<IEnumerable<PostDTO>> GetAsync();
    public Task<PostWithUserAndCommentsDTO> GetByIdAsync(int id);
    public Task<PostDTO?> PostAsync(CreatePostDTO create_post_dto);
    public Task<PostDTO?> UpdateAsync(CreatePostDTO create_post_dto, int id);
}