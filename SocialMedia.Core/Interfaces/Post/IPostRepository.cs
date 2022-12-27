namespace SocialMedia.Core.Interfaces.Post;
public interface IPostRepository
{
    public Task<IEnumerable<PostDTO>> GetAsync();
    public Task<PostWithUserAndCommentsDTO> GetByIdAsync(int id);
    public Task<PostDTO?> PostAsync(CreatePostDTO post);
    public Task<PostDTO?> UpdateAsync(CreatePostDTO post, int id);
    public Task<bool> DeleteAsync(int id);
}
