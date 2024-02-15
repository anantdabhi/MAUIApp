using System.ComponentModel.DataAnnotations.Schema;

namespace ChatAppApi.Entities;

[Table("tblmessages")]
public class TblMessage
{
    [Column("id")] public int Id { get; set; }

    [Column("fromuserid")] public int FromUserId { get; set; }

    [Column("touserid")] public int ToUserId { get; set; }

    [Column("content")] public string Content { get; set; } = null;

    [Column("senddateday")] public string SendDateDay { get; set; }

    [Column("senddatetime")] public string SendDateTime { get; set; }

    [Column("isread")] public bool IsRead { get; set; }
}