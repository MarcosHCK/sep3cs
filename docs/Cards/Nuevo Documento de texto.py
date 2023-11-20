import requests
from bs4 import BeautifulSoup
import os
import os

directorio_script = os.path.dirname(os.path.realpath(__file__))
import re

def limpiar_nombre_archivo(url):
    return re.sub(r'\W+', '', url.split("/")[-1])

import os
import requests

def descargar_imagen(url):
    respuesta = requests.get(url, stream=True)
    nombre_archivo = limpiar_nombre_archivo(os.path.basename(url))
    ruta_archivo = os.path.join(directorio_script, nombre_archivo)
    
    with open(ruta_archivo, 'wb') as archivo:
        for chunk in respuesta.iter_content(chunk_size=128):
            archivo.write(chunk)
    
    print(f"Descargada imagen: {ruta_archivo}")


def obtener_imagenes_cartas():
    url = "https://clashroyale.fandom.com/wiki/Category:Card_Images"  # URL de la página con las imágenes
    respuesta = requests.get(url)
    soup = BeautifulSoup(respuesta.text, 'html.parser')
    
    for img in soup.findAll("img"):
        descargar_imagen(img["src"])

obtener_imagenes_cartas()
def cambiar_extension_a_png(directorio):
    for nombre_archivo in os.listdir(directorio):
        # Ignorar los archivos que ya son .png
        if not nombre_archivo.endswith('.png'):
            ruta_completa = os.path.join(directorio, nombre_archivo)
            nombre_base = os.path.splitext(nombre_archivo)[0]  # Obtener el nombre del archivo sin la extensión
            nueva_ruta = os.path.join(directorio, nombre_base + '.png')
            os.rename(ruta_completa, nueva_ruta)

cambiar_extension_a_png(directorio_script)