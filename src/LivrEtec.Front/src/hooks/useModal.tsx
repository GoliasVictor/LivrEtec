// src/hooks/useAuth.jsx

import React, {  createContext, PropsWithChildren, ReactNode, useContext, useMemo, useState } from "react";


const ModalContext = createContext<ModalContextData | null>(null);
type ModalContextData = {
  modal: ReactNode | null;
  setModal: (modal: ReactNode) => void;
  closeModal: () => void;
}
export const ModalProvider = ({ children } : PropsWithChildren) => {
    
  const [innerModal, setInnerModal] = useState<ReactNode | null>(null);

  const setModal = (modal: ReactNode) => {
    setInnerModal(modal)
  }
  const closeModal = () => {
    setInnerModal(null)
  }
  const value = useMemo(() => ({
      modal: innerModal,
    setModal: setModal,
      closeModal: closeModal
    }), [innerModal])
  

  return <ModalContext.Provider value={value}>{children}</ModalContext.Provider>;
};

export const useModal = () => {
  return useContext(ModalContext);
};