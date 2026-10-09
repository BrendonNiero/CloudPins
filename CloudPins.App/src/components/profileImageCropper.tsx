import { Button } from "@heroui/button";
import { Modal, ModalBody, ModalContent, ModalFooter, ModalHeader } from "@heroui/modal";
import Cropper, { Area } from "react-easy-crop";
import { useEffect, useState } from "react";

type ProfileImageCropperProps = {
  file: File | null;
  isOpen: boolean;
  onClose: () => void;
  onComplete: (file: File, previewUrl: string) => void;
};

async function cropImage(imageSource: string, area: Area, name: string): Promise<File> {
  const image = await new Promise<HTMLImageElement>((resolve, reject) => {
    const element = new Image();
    element.onload = () => resolve(element);
    element.onerror = reject;
    element.src = imageSource;
  });

  const canvas = document.createElement("canvas");
  canvas.width = area.width;
  canvas.height = area.height;
  const context = canvas.getContext("2d");
  if (!context) throw new Error("Não foi possível preparar o recorte da imagem.");

  context.drawImage(image, area.x, area.y, area.width, area.height, 0, 0, area.width, area.height);
  const blob = await new Promise<Blob | null>((resolve) => canvas.toBlob(resolve, "image/jpeg", 0.92));
  if (!blob) throw new Error("Não foi possível recortar a imagem.");
  return new File([blob], `${name.replace(/\.[^/.]+$/, "")}-perfil.jpg`, { type: "image/jpeg" });
}

export default function ProfileImageCropper({ file, isOpen, onClose, onComplete }: ProfileImageCropperProps) {
  const [imageUrl, setImageUrl] = useState("");
  const [crop, setCrop] = useState({ x: 0, y: 0 });
  const [zoom, setZoom] = useState(1);
  const [croppedArea, setCroppedArea] = useState<Area | null>(null);
  const [isSaving, setIsSaving] = useState(false);

  useEffect(() => {
    if (!file) return;
    const url = URL.createObjectURL(file);
    setImageUrl(url);
    setCrop({ x: 0, y: 0 });
    setZoom(1);
    return () => URL.revokeObjectURL(url);
  }, [file]);

  async function handleConfirm() {
    if (!file || !croppedArea) return;
    try {
      setIsSaving(true);
      const croppedFile = await cropImage(imageUrl, croppedArea, file.name);
      onComplete(croppedFile, URL.createObjectURL(croppedFile));
      onClose();
    } finally {
      setIsSaving(false);
    }
  }

  return (
    <Modal isOpen={isOpen} onOpenChange={(open) => !open && onClose()} placement="center" size="lg" backdrop="blur" hideCloseButton>
      <ModalContent>
        <ModalHeader className="flex flex-col gap-1 text-[#092b5a]">Ajustar foto de perfil</ModalHeader>
        <ModalBody>
          <p className="text-sm text-default-500">Posicione a foto dentro do círculo.</p>
          <div className="relative h-80 overflow-hidden rounded-2xl bg-black">
            {imageUrl && <Cropper image={imageUrl} crop={crop} zoom={zoom} aspect={1} cropShape="round" showGrid={false} onCropChange={setCrop} onZoomChange={setZoom} onCropComplete={(_, pixels) => setCroppedArea(pixels)} />}
          </div>
          <label className="flex items-center gap-3 text-sm font-medium text-[#0b3a75]">
            Zoom
            <input className="w-full accent-[#0877df]" type="range" min={1} max={3} step={0.1} value={zoom} onChange={(event) => setZoom(Number(event.target.value))} />
          </label>
        </ModalBody>
        <ModalFooter>
          <Button variant="light" onPress={onClose}>Cancelar</Button>
          <Button color="primary" variant="shadow" isLoading={isSaving} onPress={handleConfirm}>Usar esta foto</Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
