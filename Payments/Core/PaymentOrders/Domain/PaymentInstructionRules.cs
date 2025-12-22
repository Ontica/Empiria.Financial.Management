/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Domain Layer                            *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Service provider                        *
*  Type     : PaymentInstructionRules                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services to control payment instruction's rules.                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments {

  /// <summary>Provides services to control payment instruction's rules.</summary>
  internal class PaymentInstructionRules {

    private PaymentInstruction _instruction;

    internal PaymentInstructionRules(PaymentInstruction instruction) {
      _instruction = instruction;
    }


    internal bool CanCancel() {
      if (_instruction.Status == PaymentInstructionStatus.Programmed ||
          _instruction.Status == PaymentInstructionStatus.Suspended) {
        return true;
      }

      return false;
    }


    internal bool CanCancelPaymentRequest() {
      if (_instruction.Status == PaymentInstructionStatus.WaitingRequest) {
        return true;
      }

      return false;
    }


    internal bool CanEditDocuments() {
      return true;
    }


    internal bool CanRequestPayment() {
      if (_instruction.Status == PaymentInstructionStatus.Programmed) {
        return true;
      }

      return false;
    }


    internal bool CanReset() {
      if (_instruction.Status == PaymentInstructionStatus.Suspended) {
        return true;
      }

      return false;
    }


    internal bool CanSuspend() {
      if (_instruction.Status == PaymentInstructionStatus.Programmed) {
        return true;
      }

      return false;
    }


    internal bool CanUpdate() {
      return false;
    }


    public bool IsReadyToBeRequested {
      get {
        if (_instruction.Status != PaymentInstructionStatus.WaitingRequest) {
          return false;
        }
        return _instruction.TimeControl.WaitingRequestTimeElapsed;
      }
    }

  }  // class PaymentInstructionRules

}  // namespace Empiria.Payments
