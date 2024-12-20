import { PropsWithChildren } from "react";
import { useModal } from "../hooks/useModal";

export default function ModalView({ children }: PropsWithChildren) {
  const m = useModal();
  if (m == null || m.modal == null) {
    return (<>{children}</>);
  }
  return (<>{m.modal}</>);
};