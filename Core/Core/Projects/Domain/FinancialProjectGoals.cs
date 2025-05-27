/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information holder                      *
*  Type     : FinancialProjectGoals                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Contains demographic and financial goals data for a financial project.                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Json;

namespace Empiria.Financial.Projects {

  /// <summary>Contains demographic and financial goals data for a financial project.</summary>
  public class FinancialProjectGoals {

    private readonly JsonObject _projectGoals;

    internal FinancialProjectGoals(JsonObject projectGoals) {

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


    public string DescripcionProyecto {
      get {
        return _projectGoals.Get("descripcionProyecto", string.Empty);
      }
      private set {
        _projectGoals.SetIfValue("descripcionProyecto", value);
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


    public int TipoCambio {
      get {
        return _projectGoals.Get("TipoCambio", 0);
      }
      private set {
        _projectGoals.SetIfValue("TipoCambio", value);
      }
    }


    public int Costo {
      get {
        return _projectGoals.Get("costo", 0);
      }
      private set {
        _projectGoals.SetIfValue("costo", value);
      }
    }


    public int Total {
      get {
        return _projectGoals.Get("total", 0);
      }
      private set {
        _projectGoals.SetIfValue("total", value);
      }
    }

    #endregion Properties

  }  // class FinancialProjectGoals

}  //namespace Empiria.Financial.Projects
