using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TrabajadoresPrueba.Models;
using TrabajadoresPrueba.Models.ViewModels;

namespace TrabajadoresPrueba.Controllers
{
    public class HomeController : Controller
    {
        private readonly TrabajadoresPruebaContext _context;

        public HomeController(TrabajadoresPruebaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Trabajadore> lista = _context.Trabajadores.Include(d => d.oDepartamento).ToList();
            return View(lista);
        }

        [HttpGet]
        public IActionResult Trabajador_Detalle(int idTrabajador)
        {
            TrabajadorVM oTrabajadorVM = new TrabajadorVM() {
                oTrabajador = new Trabajadore(),
                oListaDepartamento = _context.Departamentos.Select(departamento => new SelectListItem()
                {
                    Text = departamento.NombreDepartamento,
                    Value = departamento.Id.ToString()

                }).ToList(),
                oListaProvincia = _context.Provincia.Select(provincia => new SelectListItem()
                {
                         Text = provincia.NombreProvincia,
                         Value = provincia.Id.ToString()

                     }).ToList(),
                      oListaDistrito = _context.Distritos.Select(distrito => new SelectListItem()
                      {
                          Text = distrito.NombreDistrito,
                          Value = distrito.Id.ToString()

                      }).ToList()
            };
            if(idTrabajador != 0) {

                oTrabajadorVM.oTrabajador = _context.Trabajadores.Find(idTrabajador);
            }

            return View(oTrabajadorVM);
        }

        [HttpPost]
        public IActionResult Trabajador_Detalle(TrabajadorVM oTrabajadorVM)
        {
            if(oTrabajadorVM.oTrabajador.Id == 0)
            {
                _context.Trabajadores.Add(oTrabajadorVM.oTrabajador);
            }
            else
            {
                _context.Trabajadores.Update(oTrabajadorVM.oTrabajador);
            }
            _context.SaveChanges();

            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public IActionResult Eliminar(int idTrabajador)
        {

            Trabajadore oTrabajador = _context.Trabajadores.Include(d => d.oDepartamento).Where(e => e.Id == idTrabajador).FirstOrDefault();

            return View(oTrabajador);
        }

        [HttpPost]
        public IActionResult Eliminar(Trabajadore oTrabajador)
        {

            _context.Trabajadores.Remove(oTrabajador);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}