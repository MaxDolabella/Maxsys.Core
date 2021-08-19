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
    public class ServiceBase<TEntity> : IServiceBase<TEntity>
        where TEntity : class
    {
        #region FIELDS

        private readonly IRepositoryBase<TEntity> _repository;
        protected readonly IValidator<TEntity> _businessValidator;
        protected readonly IValidator<TEntity> _persistenceValidator;
        protected bool _disposed;
        protected readonly bool _hasValidators;

        #endregion FIELDS

        #region CONSTRUCTORS

        public ServiceBase(IRepositoryBase<TEntity> repository
            , IValidator<TEntity> businessValidator
            , IValidator<TEntity> persistenceValidator)
        {
            _disposed = false;
            _repository = repository;
            _businessValidator = businessValidator;
            _persistenceValidator = persistenceValidator;

            _hasValidators = true;
        }

        public ServiceBase(IRepositoryBase<TEntity> repository)
            : this(repository, null, null) { _hasValidators = false; }

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
            ValidationResult validationResult = null;

            if (_hasValidators)
            {
                #region Business

                validationResult = _businessValidator.Validate(entity);
                if (!validationResult.IsValid)
                    return validationResult;

                #endregion Business

                #region Persistence

                validationResult = _persistenceValidator.Validate(entity);
                if (!validationResult.IsValid)
                    return validationResult;

                #endregion Persistence
            }

            // oh! It's all ok
            var added = _repository.Add(entity);

            if (!added)
                validationResult.AddFailure($"{entity}", $"{nameof(TEntity)} could not be added.");

            return validationResult ?? new ValidationResult();
        }

        ///<inheritdoc/>
        public virtual ValidationResult Update(TEntity entity)
        {
            ValidationResult validationResult = null;

            if (_hasValidators)
            {
                #region Business

                validationResult = _businessValidator.Validate(entity);
                if (!validationResult.IsValid)
                    return validationResult;

                #endregion Business

                #region Persistence

                validationResult = _persistenceValidator.Validate(entity);
                if (!validationResult.IsValid)
                    return validationResult;

                #endregion Persistence
            }

            var updated = _repository.Update(entity);

            if (!updated)
                validationResult.AddFailure($"{entity}", $"{nameof(TEntity)} could not be updated.");

            return validationResult ?? new ValidationResult();
        }

        ///<inheritdoc/>
        public virtual ValidationResult Remove(object id)
        {
            var validationResult = new ValidationResult();

            /* Old version 2021-07-16
            var entityForDeleting = _repository.GetById(id);
            if (entityForDeleting == null)
                validationResult.AddFailure($"{id}"
                    , $"{nameof(TEntity)} with ID[{id}], does not exist in database.");
            else
                _repository.Remove(entityForDeleting);
            */

            // New version 2021-07-16
            var removed = _repository.Remove(id);

            if (!removed)
                validationResult.AddFailure($"{id}", $"{nameof(TEntity)} with ID[{id}], could not be deleted.");

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

            if (_hasValidators)
            {
                #region Business

                validationResult = _businessValidator.Validate(entity);
                if (!validationResult.IsValid)
                    return validationResult;

                #endregion Business

                #region Persistence

                validationResult = _persistenceValidator.Validate(entity);
                if (!validationResult.IsValid)
                    return validationResult;

                #endregion Persistence
            }

            // oh! It's all ok
            var added = await _repository.AddAsync(entity);

            if (!added)
                validationResult.AddFailure($"{entity}", $"{nameof(TEntity)} could not be added.");

            return validationResult;
        }

        ///<inheritdoc/>
        public virtual async Task<ValidationResult> UpdateAsync(TEntity entity)
        {
            ValidationResult validationResult = new ValidationResult();

            if (_hasValidators)
            {
                #region Business

                validationResult = _businessValidator.Validate(entity);
                if (!validationResult.IsValid)
                    return validationResult;

                #endregion Business

                #region Persistence

                validationResult = _persistenceValidator.Validate(entity);
                if (!validationResult.IsValid)
                    return validationResult;

                #endregion Persistence
            }

            var updated = await _repository.UpdateAsync(entity);

            if (!updated)
                validationResult.AddFailure($"{entity}", $"{nameof(TEntity)} could not be updated.");

            return validationResult;
        }

        ///<inheritdoc/>
        public virtual async Task<ValidationResult> RemoveAsync(object id)
        {
            var validationResult = new ValidationResult();

            /* Old version 2021-07-16
            var validationResult = new ValidationResult();

            var entityForDeleting = await _repository.GetByIdAsync(id);

            if (entityForDeleting == null)
                validationResult.AddFailure($"{id}"
                    , $"{nameof(TEntity)} with ID[{id}], does not exist in database.");
            else
                await _repository.RemoveAsync(entityForDeleting);
            */

            // New version 2021-07-16
            var removed = await _repository.RemoveAsync(id);

            if (!removed)
                validationResult.AddFailure($"{nameof(TEntity)} with ID[{id}], could not be deleted.");

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
                    _repository.Dispose();

                    if (_businessValidator != null)
                        GC.SuppressFinalize(_businessValidator);

                    if (_persistenceValidator != null)
                        GC.SuppressFinalize(_persistenceValidator);
                }
            }
            _disposed = true;
        }

        #endregion Dispose
    }
}