import "../App.css";
import { PropsWithChildren } from "react";

interface ConfirmarModalProps extends PropsWithChildren {
  onClose: () => void
  onConfirm: () => void
  mensagem: string
}
export default function ConfirmarModal({ mensagem, children, onClose, onConfirm}: ConfirmarModalProps) {
  const handleCancel = () => {
    onClose();
  }
  const handleConfirm = () => {
    onConfirm()
    onClose();
  }
  return (<> 
    {children}
    <form className="flex flex-col border-2" onSubmit={handleConfirm}>
      {mensagem}
      <div className="flex flex-row w-fill justify-between">
        <button className="m-2" type="button" onClick={handleCancel}> Cancelar </button>
        <button className="m-2"> Confirmar </button>
      </div>
    </form>
  </>)
};