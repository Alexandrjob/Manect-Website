namespace Manect.Data.Entities
{
    /// <summary>
    /// Статус обьекта в бд.
    /// </summary>
    public enum Status
    {
        Created,
        Deleted,
        Modified,
        Completed,
        Received
    }

    public enum StatusRus
    {
        Создал,
        Удалил,
        Изменил,
        Выполнил,
        Скачал
    }
}
