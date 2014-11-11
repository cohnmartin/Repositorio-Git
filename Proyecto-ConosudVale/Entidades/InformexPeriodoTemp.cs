using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    public class InformexPeriodoTemp
    {
        string _RazonSocial ;
        DateTime _Periodo ;
        string _PeriodoDesc;
        string _Comentario ;
        string _Tipo;
        bool _EsContratista ;
        int _OrdenDetalle;
        int _OrdenEmpresas;


        public int OrdenDetalle
        {
            get { return _OrdenDetalle; }
            set { _OrdenDetalle = value; }
        }

        public int OrdenEmpresas
        {
            get { return _OrdenEmpresas; }
            set { _OrdenEmpresas = value; }
        }


        public string RazonSocial
        {
            get { return _RazonSocial; }
            set { _RazonSocial = value; }
        }
        public DateTime Periodo
        {
            get { return _Periodo; }
            set { _Periodo = value; }
        }
        public string PeriodoDesc
        {
            get { return _PeriodoDesc; }
            set { _PeriodoDesc = value; }
        }
        public string Comentario
        {
            get { return _Comentario; }
            set { _Comentario = value; }
        }
        public string Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }
        public bool EsContratista
        {
            get { return _EsContratista; }
            set { _EsContratista = value;}
        }
    }
}
