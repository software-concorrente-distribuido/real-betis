import socket
import tkinter as tk
from tkinter import messagebox
import base64
import json
from PIL import Image, ImageTk
import io

class ClientInterface:
    def __init__(self, master):
        self.master = master
        self.master.title("Client Interface")

        self.label = tk.Label(master, text="Enter image number (1-5):")
        self.label.pack()

        self.entry = tk.Entry(master)
        self.entry.pack()

        self.button = tk.Button(master, text="Send Request", command=self.send_request)
        self.button.pack()

        self.image_label = tk.Label(master)
        self.image_label.pack()

    def send_request(self):
        image_number = self.entry.get()
        if not image_number.isdigit() or not (1 <= int(image_number) <= 5):
            messagebox.showerror("Error", "Please enter a number between 1 and 5")
            return

        request = f"image {image_number}".encode('utf-8')
        response = self.communicate_with_server(request)
        if response:
            self.display_image(response)

    def communicate_with_server(self, request):
        try:
            # Conecte-se ao servidor
            client_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            client_socket.connect(('realbetis.software', 9090))

            # Envie a solicitação
            client_socket.sendall(request)

            # Receba a resposta
            response = b""
            while True:
                part = client_socket.recv(1024)
                if not part:
                    break
                response += part

            client_socket.close()
            return response
        except Exception as e:
            messagebox.showerror("Error", f"Failed to communicate with server: {e}")
            return None

    def display_image(self, response):
        try:
            response_json = json.loads(response)
            image_data = base64.b64decode(response_json['image'])
            image = Image.open(io.BytesIO(image_data))
            photo = ImageTk.PhotoImage(image)
            self.image_label.config(image=photo)
            self.image_label.image = photo
        except Exception as e:
            messagebox.showerror("Error", f"Failed to display image: {e}")

if __name__ == "__main__":
    root = tk.Tk()
    client_interface = ClientInterface(root)
    root.mainloop()
