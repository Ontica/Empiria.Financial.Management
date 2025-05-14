/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Account                            Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Output DTO                            *
*  Type     : AccountFields                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input field with financial account data.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Financial.Accounts.Adapters {

  /// <summary>Input field with financial account data.</summary>
  public class FinancialAccountFields {

    public int TypeId {
      get; set;
    } = 0;


    public string StandarAccountUID {
      get; set;
    } = string.Empty;


    public string CategoryUID {
      get; set;
    } = string.Empty;


    public string OrganizationUnitUID {
      get; set;
    } = string.Empty;


    public string PartyUID {
      get; set;
    } = string.Empty;


    public string ProjectUID {
      get; set;
    } = string.Empty;


    public int LedgerId {
      get; set;
    } = -1;


    public string AcctNo {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string Identifiers {
      get; set;
    } = string.Empty;


    public string Tags {
      get; set;
    } = string.Empty;


    public string Attributes {
      get; set;
    } = string.Empty;


    public string FinancialData {
      get; set;
    } = string.Empty;


    public string ConfigData {
      get; set;
    } = string.Empty;


    public int ParentId {
      get; set;
    } = 0;



    public EntityStatus Status {
      get; set;
    } = EntityStatus.Active;

    internal void EnsureValid() {
      Assertion.Require(AcctNo, "Necesito el numero de cuenta.");
      Assertion.Require(Description, "Necesito el nombre de cuenta.");
      Assertion.Require(StandarAccountUID, "Necesito la cuenta estandar.");
      Assertion.Require(CategoryUID, "Necesito la categoria del cuenta.");
      Assertion.Require(OrganizationUnitUID, "Necesito el area.");
      _ = Party.Parse(OrganizationUnitUID);
    }
  }  // class AccountFields

}  // namespace Empiria.Financial.Accounts.Adapters
