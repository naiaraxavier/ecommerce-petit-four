using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace TrabalhoHowV.Repositorios
{
    public abstract class RepositorioDeClientes<TEntity, Tkey> where TEntity : class
    {
        protected string con { get; } = WebConfigurationManager.ConnectionStrings["HowTeste"].ConnectionString;

        public abstract List<TEntity> GetAll();
        public abstract TEntity GetById(Tkey id);
        public abstract void Save(TEntity entity);
        public abstract void Update(TEntity entity);
        public abstract void Delete(TEntity entity);
        public abstract void DeleteById(Tkey id);
    }
}