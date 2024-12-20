import "../App.css";
import { PropsWithChildren } from "react";
import { useModal } from "../hooks/useModal";

interface ConfirmarModalProps extends PropsWithChildren {
  onConfirm: () => void
}
export default function ConfirmarModal({children, onConfirm}: ConfirmarModalProps) {
  const { closeModal } = useModal()!;
  const handleCancel = () => {
    closeModal();
  }
  const handleConfirm = () => {
    onConfirm()
    closeModal();
  }
  return (<> 
    <form className="flex flex-col border-2" onSubmit={handleConfirm}>
      {children}
      <div className="flex flex-row w-fill justify-between">
        <button className="m-2" type="button" onClick={handleCancel}> Cancelar </button>
        <button className="m-2"> Confirmar </button>
      </div>
    </form>
  </>)
};