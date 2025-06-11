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

    private readonly JsonObject _projectGoals = new JsonObject();

    internal FinancialProjectGoals(JsonObject projectGoals) {
      Assertion.Require(projectGoals, nameof(projectGoals));

      _projectGoals = projectGoals;
    }

    #region Properties

    public string Beneficiario {
      get {
        return _projectGoals.Get("beneficiario", string.Empty);
      }
      private set {
        _projectGoals.SetIfValue("beneficiario", value);
      }
    }


    public string Localizacion {
      get {
        return _projectGoals.Get("localizacion", string.Empty);
      }
      private set {
        _projectGoals.SetIfValue("localizacion", value);
      }
    }


    public int PoblacionBeneficiada {
      get {
        return _projectGoals.Get("poblacionBeneficiada", 0);
      }
      private set {
        _projectGoals.SetIfValue("poblacionBeneficiada", value);
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


    public decimal Costo {
      get {
        return _projectGoals.Get("costo", 0m);
      }
      private set {
        _projectGoals.SetIfValue("costo", value);
      }
    }

    #endregion Properties

    #region Helpers

    internal string ToJsonString() {
      return _projectGoals.ToString();
    }

    #endregion Helpers

  }  // class FinancialProjectGoals

}  //namespace Empiria.Financial.Projects
