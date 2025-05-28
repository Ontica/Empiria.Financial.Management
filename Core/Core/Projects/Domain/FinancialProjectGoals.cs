/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information holder                      *
*  Type     : FinancialProjectGoals                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Contains demographic and financial goals data for a financial project.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;
using Empiria.Json;

namespace Empiria.Financial.Projects {

  /// <summary>Contains demographic and financial goals data for a financial project.</summary>
  public class FinancialProjectGoals {

    private readonly JsonObject _projectGoals = new JsonObject();

    internal FinancialProjectGoals(JsonObject projectGoals) {
      Assertion.Require(projectGoals, nameof(projectGoals));

      _projectGoals = projectGoals;
    }

    #region Properties
   
    public int ClaveObra {
      get {
        return _projectGoals.Get("claveObra", 0);
      }
      private set {
        _projectGoals.SetIfValue("claveObra", value);
      }
    }


    public string Acreditado {
      get {
        return _projectGoals.Get("acreditado", string.Empty);
      }
      private set {
        _projectGoals.SetIfValue("acreditado", value);
      }
    }

  
    public string Municipio {
      get {
        return _projectGoals.Get("municipio", string.Empty);
      }
      private set {
        _projectGoals.SetIfValue("municipio", value);
      }
    }


    public int TipoCreedito {
      get {
        return _projectGoals.Get("tipoCreedito", 0);
      }
      private set {
        _projectGoals.SetIfValue("tipoCreedito", value);
      }
    }


    public string TipoObra {
      get {
        return _projectGoals.Get("tipoObra", string.Empty);
      }
      private set {
        _projectGoals.SetIfValue("tipoObra", value);
      }
    }  


    public int Beneficiarios {
      get {
        return _projectGoals.Get("beneficiarios", 0);
      }
      private set {
        _projectGoals.SetIfValue("beneficiarios", value);
      }
    }


    public string UnidadBeneficiaria {
      get {
        return _projectGoals.Get("unidadBeneficiaria", string.Empty);
      }
      private set {
        _projectGoals.SetIfValue("unidadBeneficiaria", value);
      }
    }


    public int EmpleosDirectos {
      get {
        return _projectGoals.Get("empleosDirectos", 0);
      }
      private set {
        _projectGoals.SetIfValue("empleosDirectos", value);
      }
    }


    public int EmpleosIndirectos {
      get {
        return _projectGoals.Get("empleosIndirectos", 0);
      }
      private set {
        _projectGoals.SetIfValue("empleosIndirectos", value);
      }
    }


    public int Moneda {
      get {
        return _projectGoals.Get("fechaAmortizacion", 0);
      }
      private set {
        _projectGoals.SetIfValue("fechaAmortizacion", value);
      }
    }


    public decimal TipoCambio {
      get {
        return _projectGoals.Get("tipoCambio", 0);
      }
      private set {
        _projectGoals.SetIfValue("tipoCambio", value);
      }
    }


    public decimal Costo {
      get {
        return _projectGoals.Get("costo", 0);
      }
      private set {
        _projectGoals.SetIfValue("costo", value);
      }
    }


    public decimal Total {
      get {
        return _projectGoals.Get("total", 0);
      }
      private set {
        _projectGoals.SetIfValue("total", value);
      }
    }

    #endregion Properties

    #region Helpers

    internal string ToJsonString() {
      return _projectGoals.ToString();
    }


    internal void Update(PojectGoalsFields fields) {
      Assertion.Require(fields, nameof(fields));

      ClaveObra = fields.ClaveObra;
      Acreditado = fields.Acreditado;
      Municipio = fields.Municipio;
      TipoCreedito = fields.TipoCreedito;
      TipoObra = fields.TipoObra;
      Beneficiarios = fields.Beneficiarios;
      UnidadBeneficiaria = fields.UnidadBeneficiaria;
      EmpleosDirectos = fields.EmpleosDirectos;
      EmpleosIndirectos = fields.EmpleosIndirectos;
      Moneda = fields.Moneda;
      TipoCambio = fields.TipoCambio;
      Costo = fields.Costo;
      Total = fields.Total;
    }

    #endregion Helpers

  }  // class FinancialProjectGoals

}  //namespace Empiria.Financial.Projects
