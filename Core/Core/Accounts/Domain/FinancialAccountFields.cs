/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Account                            Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Input fields DTO                      *
*  Type     : FinancialAccountFields                       License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input fields used to create or update financial accounts.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Newtonsoft.Json;

using Empiria.Parties;

using Empiria.Financial.Projects;

namespace Empiria.Financial {

  /// <summary>Input fields used to create or update financial accounts.</summary>
  public class FinancialAccountFields {

    public FinancialAccountFields() {
      JsonConvert.DefaultSettings = () => Json.JsonConverter.JsonSerializerDefaultSettings();
    }

    #region Properties

    public string UID {
      get;  set;
    } = string.Empty;


    public string FinancialAccountTypeUID {
      get; set;
    } = string.Empty;


    public string AccountNo {
      get; set;
    } = string.Empty;


    public string Description {
      get; set;
    } = string.Empty;


    public string StandardAccountUID {
      get; set;
    } = string.Empty;


    public string OrganizationalUnitUID {
      get; set;
    } = string.Empty;


    public string CurrencyUID {
      get; set;
    } = string.Empty;


    public string ProjectUID {
      get; set;
    } = string.Empty;


    public string[] Tags {
      get; set;
    } = new string[0];


    public object Attributes {
      get; set;
    } = new object();


    public object FinancialData {
      get; set;
    } = new object();

    #endregion Properties

    #region Methods

    internal void EnsureValid() {

      AccountNo = FieldPatcher.Clean(AccountNo);
      Description = FieldPatcher.Clean(Description);

      UID = FieldPatcher.Clean(UID);
      FinancialAccountTypeUID = FieldPatcher.Clean(FinancialAccountTypeUID);
      StandardAccountUID = FieldPatcher.Clean(StandardAccountUID);
      OrganizationalUnitUID = FieldPatcher.Clean(OrganizationalUnitUID);
      CurrencyUID = FieldPatcher.Clean(CurrencyUID);
      ProjectUID = FieldPatcher.Clean(ProjectUID);

      if (UID.Length != 0) {
        _ = FinancialAccount.Parse(UID);
      };

      if (FinancialAccountTypeUID.Length != 0) {
        _ = FinancialAccountType.Parse(FinancialAccountTypeUID);
      }

      if (StandardAccountUID.Length != 0) {
        _ = StandardAccount.Parse(StandardAccountUID);
      }

      if (OrganizationalUnitUID.Length != 0) {
        _ = OrganizationalUnit.Parse(OrganizationalUnitUID);
      }

      if (CurrencyUID.Length != 0) {
        _ = Currency.Parse(CurrencyUID);
      }

      if (ProjectUID.Length != 0) {
        _ =  FinancialProject.Parse(ProjectUID);
      }
    }

    #endregion Methods

  }  // class FinancialAccountFields

}  // namespace Empiria.Financial
