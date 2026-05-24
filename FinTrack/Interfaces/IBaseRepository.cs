using FinTrack.Core.Entities;

namespace FinTrack.Core.Interfaces
{
    /// <summary>
    /// Defina el contrato básico y genérico de una [Entidad] repositorio para las operaciones CRUD.
    /// </summary>
    /// <remarks>
    /// Proporciona las firmas de los métodos asíncronos y síncronos elementales para interactuar con cualquier entidad que herede de BaseEntity.
    /// </remarks>
    /// <typeparam name="T">El tipo de entidad de negocio que maneja el repositorio.</typeparam>
    public interface IBaseRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Recupera de forma asíncrona todos los registros de la entidad correspondientes en la base de datos.
        /// </summary>
        /// <returns>Una colección con todos los elementos de tipo <typeparamref name="T"/>.</returns>
        Task<IEnumerable<T>> GetAll();

        /// <summary>
        /// Obtiene un registro específico mediante su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del registro.</param>
        /// <returns>La entidad de tipo <typeparamref name="T"/> encontrada.</returns>
        Task<T> GetById(int id);

        /// <summary>
        /// Registra o inserta una nueva entidad de forma asíncrona en el contexto de persistencia.
        /// </summary>
        /// <param name="entity">El objeto entidad que se desea guardar.</param>
        Task Insert(T entity);

        /// <summary>
        /// Prepara y marca las modificaciones realizadas en una entidad existente dentro del contexto.
        /// </summary>
        /// <param name="entity">El objeto entidad modificado que se desea actualizar.</param>
        void Update(T entity);

        /// <summary>
        /// Remueve o elimina lógicamente un registro del almacén de datos mediante su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del registro a eliminar.</param>
        Task Delete(int id);
    }
}