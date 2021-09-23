using FluentValidation;
using FluentValidation.Results;
using Maxsys.ModelCore.Interfaces.Repositories;
using Maxsys.ModelCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maxsys.ModelCore.Services
{
    /// <summary>
    /// Provides a class for basic entity crud operations.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <inheritdoc cref="IServiceBase{TEntity}"/>
    public abstract class ServiceBase<TEntity> : IServiceBase<TEntity>
        where TEntity : class
    {
        #region FIELDS

        private readonly IRepositoryBase<TEntity> _repository;
        //protected readonly ICollection<object> _componentsToClear = new HashSet<object>();
        protected bool _disposed;

        #endregion FIELDS

        #region CONSTRUCTORS

        protected ServiceBase(IRepositoryBase<TEntity> repository)
        {
            _disposed = false;
            _repository = repository;
        }

        #endregion CONSTRUCTORS

        #region CRUD SYNC

        ///<inheritdoc/>
        public virtual TEntity GetById(object id, bool @readonly = true)
            => _repository.GetById(id, @readonly);

        ///<inheritdoc/>
        public virtual IEnumerable<TEntity> GetAll(bool @readonly = true)
            => _repository.GetAll(@readonly);

        ///<inheritdoc/>
        public virtual ValidationResult Add(TEntity entity)
        {
            var validationResult = new ValidationResult();

            var isAdded = _repository.Add(entity);

            if (!isAdded) validationResult.AddFailure($"{nameof(TEntity)} could not be added.");

            return validationResult;
        }

        ///<inheritdoc/>
        public virtual ValidationResult Add(TEntity entity, IValidator<TEntity> validator)
        {
            if (validator is null) throw new ArgumentNullException(nameof(validator));

            var validationResult = validator.Validate(entity);

            return validationResult.IsValid
                ? Add(entity)
                : validationResult;
        }

        ///<inheritdoc/>
        public virtual ValidationResult Update(TEntity entity)
        {
            var validationResult = new ValidationResult();

            var isUpdated = _repository.Update(entity);

            if (!isUpdated) validationResult.AddFailure($"{nameof(TEntity)} could not be updated.");

            return validationResult;
        }

        ///<inheritdoc/>
        public virtual ValidationResult Update(TEntity entity, IValidator<TEntity> validator)
        {
            if (validator is null) throw new ArgumentNullException(nameof(validator));

            var validationResult = validator.Validate(entity);

            return validationResult.IsValid
                ? Update(entity)
                : validationResult;
        }

        ///<inheritdoc/>
        public virtual ValidationResult Remove(object id)
        {
            var validationResult = new ValidationResult();

            var isRemoved = _repository.Remove(id);

            if (!isRemoved) validationResult.AddFailure($"{nameof(TEntity)} with ID[{id}], could not be deleted.");

            return validationResult;
        }

        #endregion CRUD SYNC

        #region CRUD ASYNC

        ///<inheritdoc/>
        public virtual async Task<TEntity> GetByIdAsync(object id, bool @readonly = true)
            => await _repository.GetByIdAsync(id, @readonly);

        ///<inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool @readonly = true)
            => await _repository.GetAllAsync(@readonly);

        ///<inheritdoc/>
        public virtual async Task<ValidationResult> AddAsync(TEntity entity)
        {
            var validationResult = new ValidationResult();
            
            var isAdded = await _repository.AddAsync(entity);
            
            if (!isAdded) validationResult.AddFailure($"{nameof(TEntity)} could not be added.");

            return validationResult;
        }

        ///<inheritdoc/>
        public virtual async Task<ValidationResult> AddAsync(TEntity entity, IValidator<TEntity> validator)
        {
            if (validator is null) throw new ArgumentNullException(nameof(validator));

            var validationResult = await validator.ValidateAsync(entity);
            
            return validationResult.IsValid 
                ? await AddAsync(entity) 
                : validationResult;
        }

        ///<inheritdoc/>
        public virtual async Task<ValidationResult> UpdateAsync(TEntity entity)
        {
            ValidationResult validationResult = new ValidationResult();

            var isUpdated = await _repository.UpdateAsync(entity);

            if (!isUpdated) validationResult.AddFailure($"{nameof(TEntity)} could not be updated.");

            return validationResult;
        }
        
        ///<inheritdoc/>
        public virtual async Task<ValidationResult> UpdateAsync(TEntity entity, IValidator<TEntity> validator)
        {
            if (validator is null) throw new ArgumentNullException(nameof(validator));

            var validationResult = await validator.ValidateAsync(entity);

            return validationResult.IsValid
                ? await UpdateAsync(entity)
                : validationResult;
        }

        ///<inheritdoc/>
        public virtual async Task<ValidationResult> RemoveAsync(object id)
        {
            var validationResult = new ValidationResult();

            var isRemoved = await _repository.RemoveAsync(id);

            if (!isRemoved) validationResult.AddFailure($"{nameof(TEntity)} with ID[{id}], could not be deleted.");

            return validationResult;
        }

        #endregion CRUD ASYNC

        #region Dispose

        ///<inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // foreach (var item in _componentsToClear)
                    //    GC.SuppressFinalize(item);
                }
            }
            _disposed = true;
        }

        #endregion Dispose
    }
}