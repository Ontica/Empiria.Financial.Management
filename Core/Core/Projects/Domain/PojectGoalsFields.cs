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
    } = 0;


    public string Acreditado {
      get; set;
    } = string.Empty;


    public string Municipio {
      get; set;
    } = string.Empty;


    public int TipoCreedito {
      get; set;
    } = 0;


    public string TipoObra {
      get; set;
    } = string.Empty;

        
    public int Beneficiarios {
      get; set;
    } = 0;


    public string UnidadBeneficiaria {
      get; set;
    } = string.Empty;


    public int EmpleosDirectos {
      get; set;
    } = 0;


    public int EmpleosIndirectos {
      get; set;
    } = 0;


    public int Moneda {
      get; set;
    } = 0;


    public decimal TipoCambio {
      get; set;
    } = 0;


    public decimal Costo {
      get; set;
    } = 0;


    public decimal Total {
      get; set;
    } = 0;

    #endregion Properties

    #region Methods

    #endregion Methods

  }  // class PojectGoalsFields

}  // namespace Empiria.Financial.Accounts
