/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Account                            Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Output DTO                            *
*  Type     : AccountFields                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input field with financial account data.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial {

  /// <summary>Input field with financial account data.</summary>
  public class FinancialAccountFields {

    #region Properties

    public string StandardAccountUID {
      get; set;
    } = string.Empty;


    public string OrganizationUID {
      get; set;
    } = string.Empty;


    public string OrganizationalUnitUID {
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

    #endregion Properties

    #region Methods

    internal void EnsureValid() {

      AcctNo = FieldPatcher.Clean(AcctNo);
      Description = FieldPatcher.Clean(Description);

      StandardAccountUID = FieldPatcher.Clean(StandardAccountUID);
      OrganizationalUnitUID = FieldPatcher.Clean(OrganizationalUnitUID);

      if (StandardAccountUID.Length != 0) {
        _ = StandardAccount.Parse(StandardAccountUID);
      }

      if (OrganizationalUnitUID.Length != 0) {
        _ = StandardAccount.Parse(OrganizationalUnitUID);
      }
    }

    #endregion Methods

  }  // class AccountFields

}  // namespace Empiria.Financial
