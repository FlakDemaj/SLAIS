WITH new_institute AS (
INSERT INTO public.institutes
(name, branch, created_at)
VALUES
    ('SAIS', 'Tech', now())
    RETURNING institute_guid
    )
INSERT INTO public.users
(email, username, first_name, last_name, password_hashed, role_id, state, login_attempts, is_blocked, created_at, fk_institute_guid)
SELECT
    'flakron.demaj@outlook.de',
    'SAIS_Server',
    'server',
    'SAIS',
    '$2a$11$HVc9Pdpxle5wieKAwxEw9uY4BqvOoX.atHQZpzb2bttU7T.YigVNK',
    4,
    0,
    0,
    false,
    now(),
    institute_guid
FROM new_institute;