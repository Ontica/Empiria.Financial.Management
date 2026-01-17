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

    static internal readonly string CAN_SET_AS_PAYED_PERMISSION = "feature-cierre-manual-pagos";

    private PaymentInstruction _instruction;

    internal PaymentInstructionRules(PaymentInstruction instruction) {
      _instruction = instruction;
    }


    internal bool CanCancel() {
      if (_instruction.Status == PaymentInstructionStatus.Programmed ||
          _instruction.Status == PaymentInstructionStatus.Suspended) {
        return true;
      }

      if (_instruction.Status == PaymentInstructionStatus.Exception &&
          _instruction.WasSent) {
        return true;
      }

      return false;
    }


    internal bool CanCancelPaymentRequest() {
      if (_instruction.Status == PaymentInstructionStatus.WaitingRequest &&
          !_instruction.WasSent) {
        return true;
      }

      if (_instruction.Status == PaymentInstructionStatus.Exception &&
          !_instruction.WasSent) {
        return true;
      }

      return false;
    }


    internal bool CanSetAsPayed() {
      if (_instruction.Status != PaymentInstructionStatus.Exception ||
          !_instruction.PaymentOrder.PaymentMethod.IsElectronic) {
        return false;
      }

      return ExecutionServer.CurrentPrincipal.HasPermission(CAN_SET_AS_PAYED_PERMISSION);
    }


    internal bool CanEditDocuments() {
      return true;
    }


    internal bool CanRequestPayment() {
      if (_instruction.Status == PaymentInstructionStatus.Programmed &&
          !_instruction.WasSent) {
        return true;
      }

      return false;
    }


    internal bool CanReset() {
      if (!_instruction.WasSent &&
         (_instruction.Status == PaymentInstructionStatus.Suspended ||
          _instruction.Status == PaymentInstructionStatus.Exception)) {
        return true;
      }

      return false;
    }


    internal bool CanSuspend() {
      if (!_instruction.WasSent &&
          (_instruction.Status == PaymentInstructionStatus.Programmed ||
          _instruction.Status == PaymentInstructionStatus.Exception)) {
        return true;
      }

      return false;
    }


    internal bool CanUpdate() {
      return false;
    }


    public bool IsReadyToBeRequested {
      get {
        if (_instruction.WasSent || _instruction.Status != PaymentInstructionStatus.WaitingRequest) {
          return false;
        }
        return _instruction.TimeControl.WaitingRequestTimeElapsed;
      }
    }


    public bool IsReadyToBeMarkedAsPayed {
      get {
        if (!_instruction.WasSent || _instruction.Status != PaymentInstructionStatus.PaymentConfirmation) {
          return false;
        }
        return _instruction.TimeControl.PaymentRequestTimeElapsed;
      }
    }

  }  // class PaymentInstructionRules

}  // namespace Empiria.Payments
