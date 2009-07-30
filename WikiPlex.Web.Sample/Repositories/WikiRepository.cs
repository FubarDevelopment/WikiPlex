using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using WikiPlex.Web.Sample.Models;

namespace WikiPlex.Web.Sample.Repositories
{
    public class WikiRepository : IWikiRepository
    {
        private readonly string connectionString;

        public WikiRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["WikiConnectionString"].ConnectionString;
        }

        public Content Get(string slug)
        {
            const string sql = @"SELECT TOP 1
                                    C.Id, C.Source, C.Version, C.VersionDate, C.TitleId, T.Name, T.Slug,
                                    (SELECT COUNT(*) FROM Content WHERE TitleId = T.Id)
                                 FROM Content C
                                 JOIN Title T ON T.Id = C.TitleId
                                 WHERE T.Slug = @Slug
                                 ORDER BY C.Version DESC";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@Slug", slug));

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (reader.Read())
                        return BuildContent(reader);
                }
            }

            return null;
        }

        public Content GetByVersion(string slug, int version)
        {
            const string sql = @"SELECT TOP 1
                                    C.Id, C.Source, C.Version, C.VersionDate, C.TitleId, T.Name, T.Slug,
                                    (SELECT COUNT(*) FROM Content WHERE TitleId = T.Id)
                                 FROM Content C
                                 JOIN Title T ON T.Id = C.TitleId
                                 WHERE T.Slug = @Slug
                                 AND C.Version = @Version";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@Slug", slug));
                cmd.Parameters.Add(new SqlParameter("@Version", version));

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (reader.Read())
                        return BuildContent(reader);
                }
            }

            return null;
        }

        public ICollection<Content> GetHistory(string slug)
        {
            const string sql = @"SELECT
                                    C.Id, C.Source, C.Version, C.VersionDate, C.TitleId, T.Name, T.Slug, 0
                                 FROM Content C
                                 JOIN Title T ON T.Id = C.TitleId
                                 WHERE T.Slug = @Slug
                                 ORDER BY C.Version DESC";

            var history = new List<Content>();

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@Slug", slug));

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    while (reader.Read())
                        history.Add(BuildContent(reader));
                }
            }

            return history;
        }

        public void Save(string slug, string title, string source)
        {
            const string sql = @"DECLARE @TitleId INT
                                 DECLARE @ContentCount INT

                                 SELECT @TitleId = T.Id, @ContentCount = (SELECT COUNT(*) FROM Content WHERE TitleId = T.Id) 
                                 FROM Title T
                                 WHERE T.Slug = @Slug

                                 IF (@TitleId IS NULL) BEGIN
                                    INSERT INTO Title (Name, Slug)
                                    VALUES (@Name, @Slug)

                                    SELECT @TitleId = SCOPE_IDENTITY()
                                 END

                                 INSERT INTO Content (TitleId, Source, Version, VersionDate)
                                 VALUES (@TitleId, @Source, ISNULL(@ContentCount, 0) + 1, GETDATE())";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add(new SqlParameter("@Slug", slug));
                cmd.Parameters.Add(new SqlParameter("@Name", title));
                cmd.Parameters.Add(new SqlParameter("@Source", source));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private static Content BuildContent(IDataRecord reader)
        {
            return new Content
                       {
                           Id = reader.GetInt32(0),
                           Source = reader.GetString(1),
                           Version = reader.GetInt32(2),
                           VersionDate = reader.GetDateTime(3),
                           Title = new Title
                                       {
                                           Id = reader.GetInt32(4),
                                           Name = reader.GetString(5),
                                           Slug = reader.GetString(6),
                                           MaxVersion = reader.GetInt32(7)
                                       }
                       };
        }
    }
}