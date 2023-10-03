using IGBot.Clients;

namespace IGBot.Models
{
    public static class Template
    {
        public static string GetInfoTemplate(string username, string website, string name, string profile_picture_url, string biography, int follows_count, int followers_count, int media_count)
        {
            return $"🏠 <b>Account name: {username}.</b>\n" +
                $"<b>General information:</b>\n" +
                $"<b>Name:</b> {name}\n" +
                $"<b>WebSite:</b> {website}\n" +
                $"<b>Profile Img:</b> <a href='{profile_picture_url}'>Image link</a>\n" +
                $"<b>Biography:</b> {biography}\n" +
                $"<b>Follows count:</b> {follows_count}\n" +
                $"<b>Followers count:</b> {followers_count}\n" +
                $"<b>Media count:</b> {media_count}";
        }

        public static string GetPostsTemplate()
        {
            return "";
        }
        public static string GetPostTemplate(Data post, int number)
        {
            string postsTemplate = "";
            postsTemplate += $"<b>Post {number}</b>\n" +
                   $"Caption: {post.caption}\n" +
                   $"Likes: {post.like_count}\n" +
                   $"Photo: <a href='{post.media_url}'>{number} image</a> \n";
            return postsTemplate;
        }
        public static string GetPostsTemplate(Data[] post)
        {
            string postsTemplate = "";
            int length = 0;
            if(post.Length>5)
            {
                length = 3;
            }
            else
            {
                length =post.Length;
            }
            for(int i=0; i<length; i++)
            {
                postsTemplate += $"<b>Post {i + 1}</b>\n" +
                   $"Caption: {post[i].caption}\n" +
                   $"Likes: {post[i].like_count}\n" +
                   $"Photo: <a href='{post[i].media_url}'>{i+1} image</a> \n";
            }
            return postsTemplate;
        }
    }
}
