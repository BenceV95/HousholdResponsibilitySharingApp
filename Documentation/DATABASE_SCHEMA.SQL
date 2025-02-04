CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(255) NOT NULL UNIQUE,
    email varchar(255) NOT NULL UNIQUE,
    first_name VARCHAR(255) NOT NULL,
    last_name VARCHAR(255) NOT NULL,
    is_admin BOOLEAN DEFAULT FALSE, -- could be derived from houshold created_by
    password_hash VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT NOW(),
    FOREIGN KEY household_id REFERENCES household(id),
);

CREATE TABLE tasks (
    id SERIAL PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    description TEXT,
    created_by INTEGER REFERENCES users(id),
    created_at TIMESTAMP DEFAULT NOW(),
    group_id INTEGER REFERENCES groups(id),
    priority BOOLEAN DEFAULT FALSE,
);

-- what tasks have been assigned to whom
CREATE TABLE scheduled_tasks (
    id SERIAL PRIMARY KEY,
    task_id INTEGER REFERENCES tasks(id),
    created_by INTEGER REFERENCES users(id),
    created_at TIMESTAMP DEFAULT NOW(),
    repeat VARCHAR(255) NOT NULL,
    specific_time TIMESTAMP,
    assigned_to INTEGER REFERENCES users(id), -- for sprint 1: only one user can be assigned to a task
);

-- what sheduled tasks have been completed , when and by whom
CREATE TABLE history (
    id SERIAL PRIMARY KEY,
    scheduled_tasks_id INTEGER REFERENCES scheduled_tasks(id),
    completed_at TIMESTAMP,
    completed_by INTEGER REFERENCES users(id),
    action BOOLEAN DEFAULT FALSE,
);

CREATE TABLE groups (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
);

CREATE TABLE household (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    created_by INTEGER REFERENCES users(id),
    created_at TIMESTAMP DEFAULT NOW(),
);

CREATE TABLE reviews (
    id SERIAL PRIMARY KEY,
    user_id INTEGER REFERENCES user(id), -- user who is being reviewed
    review TEXT NOT NULL,
    rating INTEGER NOT NULL,
    created_at TIMESTAMP DEFAULT NOW(),
    created_by INTEGER REFERENCES users(id),
);