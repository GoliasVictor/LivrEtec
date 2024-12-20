// src/hooks/useAuth.jsx

import React, {  createContext, PropsWithChildren, ReactNode, useContext, useMemo, useState } from "react";


const ModalContext = createContext<ModalContextData | null>(null);
type ModalContextData = {
  modal: ReactNode | null
  setModal: (modalBuilder: (onClose: () => void) => ReactNode) => void;
}
export const ModalProvider = ({ children } : PropsWithChildren) => {
    
  const [innerModal, setInnerModal] = useState<ReactNode | null>(null);

  const setModal = (modalBuilder: (onClose: () => void ) => ReactNode) => {
    let modal = modalBuilder(() => {
      setInnerModal(null)
    })
    setInnerModal(modal)
  }
  const value = useMemo(() => ({
      modal: innerModal,
      setModal: setModal
    }), [innerModal])
  

  return <ModalContext.Provider value={value}>{children}</ModalContext.Provider>;
};

export const useModal = () => {
  return useContext(ModalContext);
};