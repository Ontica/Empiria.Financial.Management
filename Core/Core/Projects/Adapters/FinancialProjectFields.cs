/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                           Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Output DTO                            *
*  Type     : FinancialProjectFields                        License   : Please read LICENSE.txt file         *
*                                                                                                            *
*  Summary  : Input field with financial projects data.                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;
using Empiria.Parties;

namespace Empiria.Financial.Projects.Adapters {

  /// <summary>Input field with financial projects data.</summary>
  public class FinancialProjectFields {

    #region Properties

    public string StandardAccountUID {
      get; set;
    } = string.Empty;


    public string CategoryUID {
      get; set;
    } = string.Empty;


    public string ProjectNo {
      get; set;
    } = string.Empty;


    public string Name {
      get; set;
    } = string.Empty;


    public string OrganizationUnitUID {
      get; set;
    } = string.Empty;


    public EntityStatus Status {
      get; set;
    } = EntityStatus.Active;

    #endregion Properties

    #region Methods

    internal void EnsureValid() {
      Assertion.Require(ProjectNo, "Necesito el numero de proyecto.");
      Assertion.Require(Name, "Necesito el nombre de proyecto.");
     
      Assertion.Require(CategoryUID, "Necesito la categoria del proyecto");
      _ = FinancialProjectCategory.Parse(CategoryUID);

      Assertion.Require(StandardAccountUID, "Necesito la cuenta estandar");
      _ = StandardAccount.Parse(StandardAccountUID);

      Assertion.Require(OrganizationUnitUID, "Necesito el area.");
      _ = Party.Parse(OrganizationUnitUID);
    }

    #endregion Methods

  }  // class FinancialProjectFields

}  // namespace Empiria.Financial.Projects.Adapters
