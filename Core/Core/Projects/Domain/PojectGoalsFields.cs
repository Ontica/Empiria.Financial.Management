/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Domain Layer                          *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Input fields                          *
*  Type     : PojectGoalsFields                            License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input fields with financial project goals information.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Financial.Projects {

  /// <summary>Input fields with financial project goals information. </summary>
  public class PojectGoalsFields {

    #region Properties

    public int ClaveObra {
      get; set;
    }


    public string Cliente {
      get; set;
    } = string.Empty;


    public string Municipio {
      get; set;
    } = string.Empty;


    public string TipoObra {
      get; set;
    } = string.Empty;


    public int Beneficiarios {
      get; set;
    }


    public string UnidadBeneficiaria {
      get; set;
    } = string.Empty;


    public int EmpleosDirectos {
      get; set;
    }


    public int EmpleosIndirectos {
      get; set;
    }
       

    public decimal Costo {
      get; set;
    }


    public decimal Total {
      get; set;
    }

    #endregion Properties

  }  // class PojectGoalsFields

}  // namespace Empiria.Financial.Accounts
