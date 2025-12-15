/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Integration Data Transfer Object        *
*  Type     : BrokerRequestDto                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Integration DTO used to send requests to payment broker providers.                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Payments.Processor.Adapters {

  /// <summary>Integration DTO used to send requests to payment broker providers.</summary>
  public class BrokerRequestDto {

    private readonly PaymentInstruction _instruction;

    internal BrokerRequestDto(PaymentInstruction instruction) {
      Assertion.Require(instruction, nameof(instruction));
      Assertion.Require(!instruction.IsEmptyInstance, nameof(instruction));
      Assertion.Require(!instruction.IsNew, "Payment instruction must be stored.");

      _instruction = instruction;
    }


    public string PaymentInstructionNo {
      get {
        return _instruction.PaymentInstructionNo;
      }
    }


    public string BeneficiaryAccountNo {
      get {
        return _instruction.PaymentOrder.PaymentAccount.CLABE;
      }
    }


    public string BeneficiaryAccountTypeCode {
      get {
        return _instruction.PaymentOrder.PaymentMethod.BrokerCode;
      }
    }


    public string BeneficiaryBankCode {
      get {
        return _instruction.PaymentOrder.PaymentAccount.Institution.BrokerCode;
      }
    }


    public string BeneficiaryName {
      get {
        return _instruction.PaymentOrder.PaymentAccount.HolderName;
      }
    }


    public string BeneficiaryTaxCode {
      get {
        return _instruction.PaymentOrder.PayTo.Code;
      }
    }


    public int PaymentRequesterId {
      get {
        return 40;
      }
    }


    public int PaymentWithdrawalAccountId {
      get {
        return 418;
      }
    }


    public string PaymentReferenceNo {
      get {
        return _instruction.PaymentOrder.ReferenceNumber;
      }
    }


    public string PaymentDescription {
      get {
        return "Pago Banobras, S.N.C.";
      }
    }


    public decimal PaymentTotal {
      get {
        return _instruction.PaymentOrder.Total;
      }
    }


    public string PaymentCurrencyCode {
      get {
        return _instruction.PaymentOrder.Currency.ISOCode;
      }
    }


    public DateTime ProgrammedDate {
      get {
        return _instruction.ProgrammedDate;
      }
    }


    public DateTime EffectiveDate {
      get {
        return _instruction.EffectiveDate;
      }
    }

  }  // class BrokerRequestDto

} // namespace Empiria.Payments.Processor.Adapters
