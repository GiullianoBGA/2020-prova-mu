using Microsoft.AspNetCore.Mvc;
using Prova.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Prova.Backend.Controllers
{
    // ControllerBase
    public class BaseController : Controller
    {
        /// <summary>
        /// Obtém a unidade de trabalho.
        /// </summary>
        protected readonly IUnitOfWork UnitOfWork;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="unitOfWork"></param>
        public BaseController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }


        #region IDisposable
        /// <summary>
        /// Sobrescreve IDisposable para forçar dispose em UnitOfWork.
        /// Conforme orientação Microsoft, sempre que um objeto implementa IDisposable
        /// ele deve ser explicitamente limpo (ao invés de aguardar o GC).
        /// </summary>
        /// <param name="disposing">True indica que recursos gerenciados devem ser explicitamente limpos; False indica que devem ser ignorados.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) { UnitOfWork.Dispose(); }
            base.Dispose(disposing);
        }
        #endregion

    }
}
