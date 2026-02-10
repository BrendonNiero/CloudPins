export type SiteConfig = typeof siteConfig;

export const siteConfig = {
  name: "CloudPins",
  description: "Onde a inspiração vira organização.",
  navItems: [
    {
      label: "Inicio",
      href: "/",
    },
    {
      label: "Cadastre-se",
      href: "/register"
    },
    {
      label: "Entrar",
      href: "/login"
    },
    {
      label: "Explorar",
      href: "/feeed/id"
    },
    {
      label: "Perfil",
      href: "/profile"
    }
  ],
  navMenuItems: [
    {
      label: "Inicio",
      href: "/",
    },
    {
      label: "Cadastre-se",
      href: "/register"
    },
    {
      label: "Entrar",
      href: "/login"
    },
    {
      label: "Explorar",
      href: "/feeed/id"
    },
    {
      label: "Perfil",
      href: "/profile"
    }
  ],
  links: {
    github: "https://github.com/BrendonNiero",
    linkedin: "https://www.linkedin.com/in/brendon-berzins-45815b268/"
  },
};
