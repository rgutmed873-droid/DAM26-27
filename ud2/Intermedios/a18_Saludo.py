
def saludar(nombre="Estudiante", idioma="es"):
    if idioma == "es":
        print(f"Hola, {nombre}")
    elif idioma == "en":
        print (f"Hello, {nombre}")
    else:
        print("Idoma no reconocido")    

saludar()
saludar("Pepe")
saludar("Pepe", "en")
