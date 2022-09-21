using System.ComponentModel.DataAnnotations.Schema;

namespace Manect.Data.Entities
{
    public class AppFile
    {
        /// <summary>
        /// Id файла.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id этапа за которым закрепятся файлы.
        /// </summary>
        [ForeignKey("Stage")]
        public int StageId { get; set; }

        /// <summary>
        /// Название Файла.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Размер файла.
        /// </summary>
        [NotMapped]
        public int Length { get; set; }

        /// <summary>
        /// Расширение файла.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Массив байтов файла.
        /// </summary>
        public byte[] Content { get; set; }
    }
}