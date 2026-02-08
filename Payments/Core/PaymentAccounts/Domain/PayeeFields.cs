/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                          Component : Domain Layer                          *
*  Assembly : Empiria.Payments.Core.dll                    Pattern   : Input fields DTO                      *
*  Type     : PayeeFields                                  License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Input fields DTO used to create and update payees.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;

namespace Empiria.Payments {

  /// <summary>Input fields DTO used to create and update payees.</summary>
  public class PayeeFields : PartyFields {

    public string UID {
      get; set;
    } = string.Empty;


    public string TypeUID {
      get; set;
    } = string.Empty;


    public string TaxCode {
      get; set;
    } = string.Empty;


    public string EmployeeNo {
      get; set;
    } = string.Empty;


    public string SubledgerAccount {
      get; set;
    } = string.Empty;


    public string SubledgerAccountName {
      get; set;
    } = string.Empty;


    internal void EnsureValid() {
      Name = EmpiriaString.Clean(Name).ToUpper();
      TaxCode = EmpiriaString.Clean(TaxCode).ToUpper();

      Assertion.Require(Name, "Requiero el nombre del beneficiario.");

      Assertion.Require(TypeUID, "Requiero el tipo de beneficiario.");

      Assertion.Require(TaxCode, "Requiero el RFC del beneficiario.");
      Assertion.Require(TaxCode.Length == 12 ||
                        TaxCode.Length == 13, "El RFC debe contener 12 o 13 caracteres.");

    }

  }  // class PayeeFields

} // namespace Empiria.Payments
