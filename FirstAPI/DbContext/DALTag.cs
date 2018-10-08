using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FirstAPI.DbContext
{
    public class DALTag
    {
        static Database1Entities db;
        public DALTag()
        {
            if (ConfigurationManager.AppSettings["Environment"].ToLower() == "prod")
                db = new Database1Entities("name=Database2Entities");
            else
                db = new Database1Entities();
        }

        public List<Tag> GetAllTagsByPlayerId(Guid playerId)
        {
            var listTag = db.Tags.Where(x => x.PlayerId == playerId).OrderBy(x => x.TagName);
            return listTag.ToList();
        }

        public int AddTag(string tagName, Guid playerId)
        {
            if (string.IsNullOrEmpty(tagName))
                return 0;
            if (playerId == null || playerId == Guid.Empty)
                return 0;

            int result = 0;
            Tag tag = new Tag();

            //Loof for existing tag by playerId
            var existingTag = db.Tags.Where(x => x.PlayerId == playerId && x.TagName == tagName).FirstOrDefault();
            if (existingTag != null)
                return 0;

            //INIT
            tag.TagName = tagName;
            tag.PlayerId = playerId;
            tag.CreationDate = DateTime.Now;
            tag.TagId = Guid.NewGuid();

            db.Tags.Add(tag);
            result = db.SaveChanges();

            return result;
        }

        public List<ChampionTag> GetAllChampionTagsByPlayerId(Guid playerId)
        {
            var tagList = db.Tags.Where(x => x.PlayerId == playerId).ToList();

            var championTagList = new List<ChampionTag>();
            if (tagList?.Count > 0)
            {
                foreach (var tag in tagList)
                {
                    var championTags = new List<ChampionTag>();
                    championTags = db.ChampionTags.Where(x => x.TagId == tag.TagId).ToList();
                    if(championTags?.Count() > 0)
                    foreach (var championTag in championTags)
                    {
                            championTagList.Add(championTag);
                    }
                }
            }
            return championTagList;
        }

        public int DeleteTag(Guid tagId, Guid playerId)
        {
            if (tagId == null || tagId == Guid.Empty)
                return 0;
            if (playerId == null || playerId == Guid.Empty)
                return 0;

            int result = 0;

            var objtoDelete = db.Tags.Where(x => x.TagId == tagId).FirstOrDefault();
            //clear all championTag with this tagId
            var clear = db.ChampionTags.Where(x => x.TagId == tagId);
            db.ChampionTags.RemoveRange(clear);
            db.SaveChanges();

            if (objtoDelete != null)
            {
                db.Tags.Remove(objtoDelete);
                result = db.SaveChanges();
            }

            return result;
        }

        public int SaveTag(List<ChampionTag> championTagList)
        {
            int result = -1;
            if (championTagList?.Count > 0)
            {
                foreach (var championTag in championTagList)
                {
                    var clear = db.ChampionTags.Where(x => x.TagId == championTag.TagId);
                    db.ChampionTags.RemoveRange(clear);
                    db.SaveChanges();
                }
                foreach (var championTag in championTagList)
                {
                    db.ChampionTags.Add(championTag);
                }
                result = db.SaveChanges();
            }
            return result;
        }
    }
}